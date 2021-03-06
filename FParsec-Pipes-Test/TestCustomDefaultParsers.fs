﻿namespace FParsec.Pipes.Test
open FParsec.Pipes.Test.Tools
open Microsoft.VisualStudio.TestTools.UnitTesting
open FParsec
open FParsec.Pipes

type CustomA =
    | CustomA of float
    static member DefaultParser =
        %% "a:" -- spaces -- +.p<float> -%> CustomA

type CustomB =
    | CustomB of char
    static member DefaultParser =
        %% "b:" -- spaces -- +.p<char> -%> CustomB

[<TestClass>]
type TestCustomDefaultParsers() =
    [<TestMethod>]
    member __.TestCustomA() =
        let parser = %p<CustomA>
        bad parser "" 0
        bad parser "a" 0
        bad parser "a: " 3
        bad parser "b: 1.5" 0
        good parser "a: 1.5" 6 (CustomA 1.5)
        good parser "a: 0" 4 (CustomA 0.0)

    [<TestMethod>]
    member __.TestCustomB() =
        let parser = %p<CustomB>
        bad parser "" 0
        bad parser "a" 0
        bad parser "b" 0
        bad parser "b: " 3
        good parser "b: 1.5" 4 (CustomB '1')
        good parser "b: 0" 4 (CustomB '0')
        good parser "b: nc" 4 (CustomB 'n')

    [<TestMethod>]
    member __.TestCustomBList() =
        let parser =
            %[
                p<CustomB>
                p<CustomB>
            ]
        bad parser "" 0
        bad parser "a" 0
        bad parser "b" 0
        bad parser "b: " 3
        good parser "b: 1.5" 4 (CustomB '1')
        good parser "b: 0" 4 (CustomB '0')
        good parser "b: nc" 4 (CustomB 'n')