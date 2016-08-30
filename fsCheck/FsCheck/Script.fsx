﻿open System
open System.Reflection
open Microsoft.FSharp.Reflection

#r "bin\Debug\FsCheck.dll"
open FsCheck
open FsCheck.Experimental

#time

type UnionCase =
    | Single
    | One of int
    | Two of string * int
    | Three of string * char * int
    | Rec of int * UnionCase

let testProp (uc : UnionCase) (uc2:UnionCase) (uc3:UnionCase) =
  true

let testPerf () =
  let config =
    {
      Config.Quick with
        MaxTest = 1000
    }
  Check.One (config, testProp)

testPerf()

let run() =
    let res = Arb.from<UnionCase>.Generator |> Gen.sample 100 100000
    sprintf "%i" res.Length

run()

type Counter(initial:int) =
    let mutable c = initial
    member __.Inc() = c <- c + 1
    member __.Dec() = c <- c- 1
    member __.Add(i:int) = c <- c + i
    member __.Get = c
    interface IDisposable with
        member x.Dispose(): unit = 
            ()
        

let spec = ObjectMachine<Counter>()
let generator = StateMachine.generate spec

let sample = generator |> Gen.sample 10 1 |> Seq.head

StateMachine.toProperty spec |> Check.Verbose

//how to generate float between 0 and 5
//let f = 
//    Arb.generate<NormalFloat>
//    |> Gen.map float
//    |> Gen.resize 5
//    |> Gen.map abs
//    |> Gen.suchThat (fun f -> f < 5.)
//    |> Gen.sample 100 10

type Arbs =
  static member SafeString () =
    Arb.Default.String () |> Arb.filter (fun str -> str <> null && not (str = ""))

  static member NonEmptyStringMaps () =
    Arb.Default.Map () |> Arb.filter (fun m -> m |> Map.toList |> List.forall (fun (k,v) -> k <> "" && k <> null))

  static member DateTime () =
    Arb.Default.DateTime ()
    |> Arb.mapFilter (fun dt -> dt.ToUniversalTime()) (fun dt -> dt.Kind = System.DateTimeKind.Utc)

let fsCheckConfig = { Config.Default with Arbitrary = [ typeof<Arbs> ] }

Check.One(fsCheckConfig,(fun (s:string,m:Map<string,int>) -> not <| String.IsNullOrEmpty s && m |> Map.forall (fun s v -> not <| String.IsNullOrEmpty s)))

Check.One(fsCheckConfig,(fun (s:DateTime) -> s.Kind = DateTimeKind.Utc))

//Gen.sample 10 1 <| Arbs.DateTime().Generator
