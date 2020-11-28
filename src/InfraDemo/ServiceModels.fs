namespace InfraDemo.ServiceModels

type ServiceName = | ServiceName of string
type Entity =
| Service of name:ServiceName


/// A model of the application on the service-level
type ServiceModel = | ServiceModel of Entity list
module ServiceModel =
    let createModel es =
        es |> ServiceModel         

    module Rules = 
        type ValidationResult =
        | Valid of ruleName:string
        | Invalid of ruleName:string * errors:ValidationError list
        and ValidationError = 
        | NonUniqueName of name:string

        let validateNamesMustBeUnique (ServiceModel entities) = 
            let ruleName = "Names must be unique"
            let nonUniques = 
                entities 
                |> List.countBy (function | Service (ServiceName name) -> name) 
                |> List.filter (fun (_, count) -> count>1)
            match List.isEmpty nonUniques with
            | true -> Valid(ruleName)
            | false -> Invalid(ruleName, nonUniques |> List.map (fun (name, _) -> NonUniqueName name))

    /// Validate the ServiceModel
    let validate sm =
        seq {
            yield Rules.validateNamesMustBeUnique sm
        }

    /// Check if the ServiceModel is valid.
    let isValid sm =
        sm 
        |> validate 
        |> Seq.exists (function 
                        | Rules.Valid(_) -> false 
                        | _ -> true) 
        |> not
