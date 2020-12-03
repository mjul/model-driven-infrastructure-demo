namespace InfraDemo.Generators


module LogicalModelGenerator = 
    open InfraDemo.Models.ServiceModels
    open InfraDemo.Models.LogicalModels

    type Environment = 
    | Environment of entitites:LogicalEntity list

    type CompilerError = | CompileError

    module NamingConventions = 
        module Queues =
            let canonicalName (s:string) : string =
                s.ToLowerInvariant().Trim()

            let queueName raw = 
                canonicalName raw |> QueueName

            let serviceInbox = function
                | ServiceName nm -> 
                    (sprintf "service-%s-inbox" nm) |> queueName

            let serviceDlq = function
                | ServiceName nm ->
                    (sprintf "service-%s-inbox-dlq" nm) |> queueName

            let serviceOutbox = function
                | ServiceName nm -> 
                    (sprintf "service-%s-outbox" nm) |> queueName

    let compileEntity entity (env:Environment) : Result<Environment,CompilerError> =
        let (Environment es) = env
        match entity with 
        | Service sname ->
            let inboxName = NamingConventions.Queues.serviceInbox sname
            let inbox =Queue.Queue inboxName
            let dlqName = NamingConventions.Queues.serviceDlq sname
            let dlq = Queue.Queue dlqName
            let outbox = Queue.Queue(NamingConventions.Queues.serviceOutbox sname)
            let fname = 
                match sname with 
                | ServiceName n -> FunctionName n
            let fn = Function.Function(fname, inbox, dlq, [], [outbox])
            let es' = es @ [Queue inbox; Queue dlq; Queue outbox; Function fn] 
            Ok (Environment es')

    let compile (sm:ServiceModel) : Result<LogicalModel,CompilerError> =
        let env0 = Environment (List.empty)
        let state0 = Ok (env0)
        let envRes : Result<Environment,CompilerError> = 
            match sm with
            | ServiceModel entities -> 
                entities |> List.fold (fun envResult entity -> envResult |> Result.bind (compileEntity entity)) state0 
        match envRes with
        | Ok (Environment entities) ->
            Ok (LogicalModel.createModel entities)
        | Error e ->
            Error e