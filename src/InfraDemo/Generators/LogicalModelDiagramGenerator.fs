namespace InfraDemo.Generators


module LogicalModelDiagramGenerator = 
    open InfraDemo.Models.LogicalModels
    open InfraDemo.Generators

    let writeDiagramTo (writer:System.IO.TextWriter) (lm:LogicalModel) =
        PlantUml.writeStartUml writer "Logical Model"

        let indent = (PlantUml.noIndent()) |> PlantUml.indentMore
        for f in (LogicalModel.findFunctions lm) do
            match f with
            | Function.Function(FunctionName(functionName), 
                                Queue.Queue(QueueName(inboxName)),
                                Queue.Queue(QueueName(dlqName)),
                                databases, 
                                outputs) ->
                let inboxV = PlantUml.genIdentifier inboxName
                let dlqV = PlantUml.genIdentifier dlqName
                let functionV = PlantUml.genIdentifier functionName
                [
                    sprintf "queue \"%s\" as %s" inboxName inboxV
                    sprintf "queue \"%s\" as %s" dlqName dlqV
                    sprintf "component \"%s\" as %s" functionName functionV
                    sprintf "%s ..> %s : \"on message\"" inboxV functionV
                    sprintf "%s ..> %s : \"on error\"" functionV dlqV
                    // TODO: databases
                    // TODO: outputs
                    ""
                ] 
                |> PlantUml.writeLines writer indent
                ignore()

        PlantUml.writeEndUml writer

    let generate (lm:LogicalModel) =
        use writer : System.IO.StringWriter = new System.IO.StringWriter()
        writeDiagramTo writer lm
        writer.ToString()
        