namespace InfraDemo.Generators

/// Primitives to generate PlantUML diagrams
module PlantUml = 
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

    /// Write an indented line to the writer
    let writeLine (writer:System.IO.TextWriter) indentation (line:string) =
        writer.Write(indentToSpaces indentation)
        writer.WriteLine(line)

    /// Write indented lines to the writer
    let writeLines (writer:System.IO.TextWriter) indentation (lines:string seq) =
        for l in lines do
            writeLine writer indentation l

    /// Generate a valid, unique identifier based on the name for use with the 'as xxx' syntax.
    let genIdentifier name =
        let simplify s = System.Text.RegularExpressions.Regex.Replace(s, @"[^a-zA-Z0-9]", "")
        sprintf "%s_%s" (simplify name) (simplify (System.Guid.NewGuid().ToString()))

    /// Write the PlantUML start-of-document marker
    let writeStartUml writer (title:string) = 
        let cleanTitle = title.Replace("\"", "")
        writeLines writer (noIndent()) [
            sprintf "@startuml \"%s\"" cleanTitle
            ""
        ]

    /// Write the PlantUML end-of-document marker
    let writeEndUml writer =        
        writeLines writer (noIndent()) [
            ""
            "@enduml"
        ]
