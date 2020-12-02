namespace InfraDemo.Generators


module ServiceDiagramGenerator = 
    open InfraDemo.Models.ServiceModels

    type Indentation = | Indentation of level:int * indent:int
    /// Starting indentation
    let noIndent () = 
        Indentation(0, 4)
    /// Increase the indentation to the next level
    let indentMore = function
        | Indentation(level, indent) -> Indentation(level+1, indent)
    /// Get the indentation as spaces corresponding to the level and per-level indent.
    let indentToSpaces =
        function 
        | Indentation(level, indent) ->
            String.replicate (level*indent) " "

    let writeLines (writer:System.IO.TextWriter) indentation (lines:string seq) =
        for l in lines do
            writer.Write(indentToSpaces indentation)
            writer.WriteLine(l)

    let umlForEntity = function
        | Service (ServiceName sn) ->
            let identifier s = System.Text.RegularExpressions.Regex.Replace(s, @"[^a-zA-Z0-9]", "")
            let interfaceName = sprintf "i_%s_%s" (identifier sn) (identifier (System.Guid.NewGuid().ToString()))
            [
                sprintf "interface \"API\" as %s" interfaceName 
                sprintf "%s - [%s]" interfaceName sn
            ] 
            |> Seq.ofList

    let writeDiagramTo (writer:System.IO.TextWriter) (sm:ServiceModel) =
        match sm with
        | ServiceModel entities ->
            // Header
            let indent = noIndent ()
            writeLines writer indent [
                "@startuml \"Service Diagram\""
                ""
            ]
            // Services
            for e in entities do
                let lines = umlForEntity e
                writeLines writer (indentMore indent) lines

            // Footer
            writeLines writer indent [
                ""
                "@enduml"
            ]
        ignore()

    let generate (sm:ServiceModel) =
        use writer : System.IO.StringWriter = new System.IO.StringWriter()
        writeDiagramTo writer sm
        writer.ToString()
        