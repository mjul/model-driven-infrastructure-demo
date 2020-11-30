namespace InfraDemoTest.Generators

open System
open Xunit

module LogicalModelGeneratorTests = 
    open InfraDemo.Models.ServiceModels
    open InfraDemo.Models.LogicalModels
    open InfraDemo.Generators

    module SmokeTest = 
        [<Fact>]
        let ``smoke test`` () =
            let sm = ServiceModel.createModel [Service("foo"|>ServiceName); Service("bar"|>ServiceName)]
            let actual = LogicalModelGenerator.compile sm
            Assert.True(
                match actual with 
                | Ok _ -> true
                | _ -> false)

    module SingleServiceTest = 

        let sm = ServiceModel.createModel [Service("foo"|>ServiceName)]

        [<Fact>]
        let ``logical model must have queues for the Service`` () =
            let (Ok lm) =  LogicalModelGenerator.compile sm

            let actual = LogicalModel.findQueues lm 

            Assert.Equal(3, List.length actual)
            let q n = Queue.Queue(QueueName n)
            Assert.True(List.contains (q "service-foo-inbox") actual)
            Assert.True(List.contains (q "service-foo-inbox-dlq") actual)
            Assert.True(List.contains (q "service-foo-outbox") actual)

        [<Fact>]
        let ``logical model must have a Function for the Service`` () =
            let (Ok lm) =  LogicalModelGenerator.compile sm

            let actual = LogicalModel.findFunctions lm 

            Assert.Equal(1, List.length actual)
            let expected = Function.Function(Queue.Queue(QueueName "service-foo-inbox"), Queue.Queue(QueueName "service-foo-inbox-dlq"), [], [Queue.Queue(QueueName "service-foo-outbox")])
            Assert.Equal(expected, List.head actual)