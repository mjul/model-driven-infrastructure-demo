namespace InfraDemoTest.Generators

open System
open Xunit

module LogicalModelGeneratorTests = 
    open InfraDemo.Models.ServiceModels
    open InfraDemo.Models.LogicalModels
    open InfraDemo.Generators

    module ServiceModelValidationTests = 
        [<Fact>]
        let ``smoke test`` () =
            let sm = ServiceModel.createModel [Service("foo"|>ServiceName); Service("bar"|>ServiceName)]
            let actual = LogicalModelGenerator.compile sm
            Assert.True(
                match actual with 
                | Ok _ -> true
                | _ -> false)
