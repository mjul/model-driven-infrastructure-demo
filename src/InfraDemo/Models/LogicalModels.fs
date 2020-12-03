namespace InfraDemo.Models.LogicalModels

type QueueName = QueueName of string

and Queue = Queue of name: QueueName

and Database = | Database

and FunctionName = FunctionName of string

and Function = Function of name:FunctionName * inbox: Queue * dlq: Queue * databases: Database list * outputs: Queue list

and LogicalEntity =
    | Queue of Queue
    | Database of Database
    | Function of Function

type LogicalModel = LogicalModel of entities: LogicalEntity list

/// Logical model of the infrastructure.
module LogicalModel =
    /// Create a new logical model with the given entities.
    let createModel es = es |> LogicalModel

    /// Find the Queues in the model
    let findQueues (lm: LogicalModel) : Queue list =
        let (LogicalModel entities) = lm
        entities |> List.choose (function | Queue q -> Some q | _ -> None)

    /// Find the Functions in the model
    let findFunctions (lm: LogicalModel) : Function list =
        let (LogicalModel entities) = lm
        entities |> List.choose (function | Function f -> Some f | _ -> None)
