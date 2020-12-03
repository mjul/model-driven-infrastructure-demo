namespace InfraDemoTest.Generators

open System
open Xunit

module LogicalModelDiagramGeneratorTests = 
    open InfraDemo.Models.ServiceModels
    open InfraDemo.Generators

    module SmokeTest = 
        [<Fact>]
        let ``smoke test`` () =
            let sm = ServiceModel.createModel [Service("foo"|>ServiceName); Service("bar"|>ServiceName)]
            let (Ok lm) = LogicalModelGenerator.compile sm
            let actual = LogicalModelDiagramGenerator.generate lm

            printfn "%s" actual

            Assert.StartsWith("@startuml", actual)
            Assert.Contains("foo-inbox", actual)
            Assert.Contains("bar-inbox", actual)
            Assert.Contains("@enduml", actual)


