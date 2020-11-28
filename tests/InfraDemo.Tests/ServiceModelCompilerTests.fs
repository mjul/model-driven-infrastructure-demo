namespace InfraDemo.Tests.Compilers

open System
open Xunit

module ServiceModelCompilerTests = 
    open InfraDemo.ServiceModels
    open InfraDemo.LogicalModels
    open InfraDemo.Compilers

    module ServiceModelValidationTests = 
        [<Fact>]
        let ``smoke test`` () =
            let sm = ServiceModel.createModel [Service("foo"|>ServiceName); Service("bar"|>ServiceName)]
            let actual = ServiceModelCompiler.compile sm
            Assert.True(
                match actual with 
                | Ok _ -> true
                | _ -> false)


