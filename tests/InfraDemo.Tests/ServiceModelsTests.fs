namespace InfraDemo.Tests

open System
open Xunit

module ServiceModelsTests = 
    open InfraDemo.ServiceModels
    open InfraDemo.ServiceModels.ServiceModel.Rules

    module ServiceModelValidationTests = 
        [<Fact>]
        let ``Validate unique names (duplicates case)`` () =
            let sm = ServiceModel.createModel (List.map (ServiceName >> Service) ["foo";"bar";"foo"])
            let actual = ServiceModel.validate sm 
            let errors = 
                actual 
                |> Seq.collect (function 
                                | Valid _ -> [] 
                                | Invalid(_, errs) -> errs) 
                |> List.ofSeq
            Assert.False(Seq.isEmpty actual)
            Assert.True(match errors with
                        | [NonUniqueName("foo")] -> true
                        | _ -> false)
            Assert.False(ServiceModel.isValid sm)

        [<Fact>]
        let ``Validate unique names (all unique case)`` () =
            let sm = ServiceModel.createModel (List.map (ServiceName >> Service) ["foo";"bar";"baz"])
            let actual = ServiceModel.validate sm 
            let invalids = 
                actual 
                |> Seq.filter (function 
                                | Valid _ -> false 
                                | _ -> true)
            Assert.True(Seq.isEmpty invalids)
            Assert.True(ServiceModel.isValid sm)
