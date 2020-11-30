namespace InfraDemo.Models.LogicalModels

type QueueName = 
    | QueueName of string
and Queue =
    | Queue of name:QueueName 
and Database =
    | Database
and Function = 
    | Function of inbox:Queue * dlq:Queue * databases:Database list * outputs:Queue list
and LogicalEntity =
    | Queue of Queue
    | Database of Database
    | Function of Function

type LogicalModel = 
    | LogicalModel of entities:LogicalEntity list

module LogicalModel =
    let createModel es =
        es |> LogicalModel
