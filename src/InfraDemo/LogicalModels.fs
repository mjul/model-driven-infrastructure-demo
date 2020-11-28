namespace InfraDemo.LogicalModels

type LogicalEntity =
    | Queue of Queue
    | Database of Database
    | Function of inbox:NormalQueue * dlq:DeadLetterQueue * databases:Database list * outputs:Queue list
and QueueName = 
    | QueueName of string
and NormalQueue = 
    | NormalQueue of name:QueueName
and DeadLetterQueue = 
    | DeadLetterQueue of name:QueueName * associatedQueue:Queue
and Queue =
    | NormalQueue of NormalQueue
    | DeadLetterQueue of DeadLetterQueue
and Database =
    | Database

type LogicalModel = 
    | LogicalModel of entities:LogicalEntity list

module LogicalModel =
    let createModel es =
        es |> LogicalModel
