namespace InfraDemo.Generators


module ServiceDiagramGenerator = 
    open InfraDemo.Models.ServiceModels
    open InfraDemo.Generators

    let umlForEntity = function
        | Service (ServiceName sn) ->
            let interfaceName = PlantUml.genIdentifier (sprintf "i_%s" sn)
            [
                sprintf "interface \"API\" as %s" interfaceName 
                sprintf "%s - [%s]" interfaceName sn
            ] 
            |> Seq.ofList

    let writeDiagramTo (writer:System.IO.TextWriter) (sm:ServiceModel) =
        match sm with
        | ServiceModel entities ->
            let indent0 = PlantUml.noIndent()
            PlantUml.writeStartUml writer "Service Diagram"
            PlantUml.writeFooterWithPageNumbers writer indent0 |> ignore
            PlantUml.writeTitle writer indent0 "Service Diagram" |> ignore

            let indent = indent0 |> PlantUml.indentMore
            for e in entities do
                let lines = umlForEntity e
                PlantUml.writeLines writer indent lines

            PlantUml.writeEndUml writer
        ignore()

    let generate (sm:ServiceModel) =
        use writer : System.IO.StringWriter = new System.IO.StringWriter()
        writeDiagramTo writer sm
        writer.ToString()
