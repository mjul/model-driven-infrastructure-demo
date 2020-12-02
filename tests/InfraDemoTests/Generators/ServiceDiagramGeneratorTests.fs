namespace InfraDemoTest.Generators

open System
open Xunit

module ServiceDiagramGeneratorTests = 
    open InfraDemo.Models.ServiceModels
    open InfraDemo.Generators

    module SmokeTest = 
        [<Fact>]
        let ``smoke test`` () =
            let sm = ServiceModel.createModel [Service("foo"|>ServiceName); Service("bar"|>ServiceName)]
            let actual = ServiceDiagramGenerator.generate sm

            printfn "%s" actual

            Assert.StartsWith("@startuml", actual)
            Assert.Contains("[foo]", actual)
            Assert.Contains("[bar]", actual)
            Assert.Contains("@enduml", actual)


