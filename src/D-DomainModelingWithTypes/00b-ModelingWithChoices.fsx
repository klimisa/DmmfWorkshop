﻿// ======================================================
// Modeling temperature as a choice
// ======================================================

type Temp =
    | F of int
    | C of float
 // | K of float  // what happens if you add this new case later?


// construct some examples
let t1 = F 101
let t2 = C 38.5

// match the two choices
let printTemperature x =
    match x with
    | F fTemp -> sprintf "%iF" fTemp
    | C cTemp -> sprintf "%fC" cTemp

// test the function
printTemperature t1
printTemperature t2


// ======================================================
// Modeling an empty wallet as a choice
// ======================================================

type MoneyAmount = MoneyAmount of float
type Wallet =
    | WithMoney of MoneyAmount
    | NoMoney

// construct some examples
let w1 = WithMoney (MoneyAmount 1.2)
let w2 = NoMoney

// match the two choices
let printWallet wallet =
    match wallet with
    | WithMoney amount -> sprintf "%A" amount
    | NoMoney -> sprintf "No Money"

// test the function
printWallet w1
printWallet w2

(*
Define a function to add money to the wallet
*)

let addMoney amount wallet =
    match wallet with
    | WithMoney (MoneyAmount money)  ->
        let newMoney =  MoneyAmount (money + amount)
        let newWallet =  WithMoney newMoney
        newWallet
    | NoMoney ->
        let newWallet =  WithMoney (MoneyAmount amount)
        newWallet

// test the addMoney function
w1 |> addMoney 4.0
w2 |> addMoney 4.0

// this is a bug! How could you fix it?
w1 |> addMoney -4.0



(*
A more complicated example -- paying with a Wallet
*)
type PaymentResult =
  | PaidSuccessfully of Wallet // return the remaining wallet
  | NotEnoughMoney
  | NoMoneyAtAll

let payWithWallet amount wallet :PaymentResult=
    match wallet with
    | WithMoney (MoneyAmount money)  ->
      if money > amount then
        let remainingMoney =  MoneyAmount (money - amount)
        let newWallet =  WithMoney remainingMoney
        PaidSuccessfully newWallet
      else if money = amount then
        let newWallet =  NoMoney
        PaidSuccessfully newWallet
      else
        NotEnoughMoney
    | NoMoney ->
        NoMoneyAtAll

let printPaymentResult result =
  match result with
  | PaidSuccessfully wallet -> printfn "Paid! Remaining in wallet: %A" wallet
  | NotEnoughMoney -> printfn "Not enough money in wallet"
  | NoMoneyAtAll -> printfn "No money in wallet"

// test the function
w1 |> payWithWallet 1.1 |> printPaymentResult
w1 |> payWithWallet 1.2 |> printPaymentResult
w1 |> payWithWallet 1.3 |> printPaymentResult
w2 |> payWithWallet 1.3 |> printPaymentResult
w2 |> addMoney 2.0 |> payWithWallet 1.3 |> printPaymentResult

// ======================================================
// Modeling optional values as a choice
// ======================================================

(*
/// this is the same definition as the built-in type
type Option<'a> =
    | Some of 'a
    | None
*)
// IMPORTANT: Reset the F# interactive session after defining!

// construct some examples of Option
let someInt = Some 1
let noInt = None

// match the two choices
let printOption opt =
    match opt with
    | Some data ->
        sprintf "Some %i" data
    | None ->
        "None"

// test the matching function
printOption someInt
printOption noInt

// ============================
// Nested matching
// ============================

type CardType = Visa | Mastercard
type CardInfo = { CardNumber : string; CardType : CardType }
type EmailAddress = EmailAddress of string
type Payment =
    | Card of CardInfo
    | Paypal of EmailAddress

// construct some examples
let visaPayment = Card {CardNumber="123"; CardType=Visa}
let mcPayment = Card {CardNumber="123"; CardType=Mastercard}
let paypalPayment = Paypal (EmailAddress "me@example.com")


// basic matching
let printPayment1 payment =
  match payment with
  | Card cardInfo ->
    // extract the fields of cardInfo with dots: .CardNumber .CardType
    printfn "Paid by Card: number: %s type: %A" cardInfo.CardNumber cardInfo.CardType
  | Paypal emailAddress ->
    // extract the inner value of the EmailAddress
    let (EmailAddress e) = emailAddress
    printfn "Paid by Paypal: email: %s " e

// extracting cardNumber/cardType/EmailAddress
// directly in the match
let printPayment2 payment =
  match payment with
  | Card {CardNumber=cardNumber; CardType=cardType} ->
    // the fields of the CardInfo are already extracted!
    printfn "Paid by Card: number: %s type: %A" cardNumber cardType
  | Paypal (EmailAddress e) ->
    // the inner value of the EmailAddress is already extracted!
    printfn "Paid by Paypal: email: %s " e

// Can also match on inner fields too.
// Here we have DIFFERENT matches for Visa vs Mastercard
let printPayment3 payment =
  match payment with
  | Card {CardNumber=cardNumber; CardType=Visa} ->
    printfn "Paid by Visa: number: %s" cardNumber
  | Card {CardNumber=cardNumber; CardType=Mastercard} ->
    printfn "Paid by Mastercard: number: %s" cardNumber
  | Paypal (EmailAddress e) ->
    printfn "Paid by Paypal: email: %s " e


// test
printPayment1 visaPayment
printPayment1 mcPayment
printPayment1 paypalPayment

// test
printPayment2 visaPayment
printPayment2 mcPayment
printPayment2 paypalPayment

// test
printPayment3 visaPayment
printPayment3 mcPayment
printPayment3 paypalPayment