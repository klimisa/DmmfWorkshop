﻿// ================================================
// FSM Exercise: A simple 2-state transition for Unverified and Verified email
//
// See "Email transition diagram.png"
//
// ================================================

// Here are the EmailAddress types from the slide
module EmailDomain =

    type EmailAddress =
        EmailAddress of string

    type VerifiedEmail =
        VerifiedEmail of EmailAddress

    type EmailContactInfo =
        | Unverified of EmailAddress
        | Verified of VerifiedEmail


    // Create a function that (maybe) creates a VerifiedEmail
    let verify (hash:string) (email:EmailAddress) :VerifiedEmail option =

        // Logic to implement:
        // if the hash is "OK", then return Some VerifiedEmail
        // else return None
        if hash="OK" then
            Some (VerifiedEmail email)
        else
            None

// ================================================
// Now write some client code that uses this domain and API
// ================================================

module EmailClient =
    open EmailDomain

    // Create a "verifyContactInfo" function that transitions
    // from Unverified state to Verified state
    let verifyContactInfo (hash:string) (email:EmailContactInfo)  :EmailContactInfo =

        // Logic to implement:
        // if the email is Unverified, then run "verify" from above
        // * if this succeeds, return the now Verified email
        // * if this doesn't succeed, return the original (unverified) email
        // else if the email is already verified,
        // * return the original (alread verified) email

        match email with
        | Unverified emailAddress ->
             let verifiedOrNone =
                emailAddress |> verify hash
             match verifiedOrNone with
             | Some verifiedEmail ->
                printfn "the email was verified"
                Verified verifiedEmail
             | None ->
                printfn "the email was not verified"
                // return original state
                email
        | Verified _ ->
            printfn "the email is already verified"
            // return original state
            email

    // Create a "sendVerificationMessage" function
    // Rule: "You can't send a verification message to a verified email"
    let sendVerificationMessage (email:EmailContactInfo) =
        // Logic to implement:
        // if the email is Unverified, then
        //   print "Sending verification email to {email}"
        // else if the email is already verified,
        //   do nothing
        match email with
        | Unverified emailAddress ->
             printfn "Sending verification email to %A" emailAddress
        | Verified _ ->
             printfn "The email is already verified"

    // Create a "sendPasswordResetMessage " function
    // Rule: "You can't send a password reset message to a unverified email "
    let sendPasswordResetMessage (email:EmailContactInfo) =
        // Logic to implement:
        // if the email is Verified, then
        //   print "Sending password reset email to {email}"
        // else if the email is Unverified
        //   do nothing
        match email with
        | Unverified emailAddress ->
             printfn "Can't send reset email to unverified %A" emailAddress
        | Verified verifiedEmailAddress ->
             printfn "Sending password reset email to %A" verifiedEmailAddress


// ================================================
// Now write some test code
// ================================================

open EmailDomain
open EmailClient

let email = EmailAddress "x@example.com"
let unverified = Unverified email

unverified |> sendVerificationMessage


let verifiedOk =
    let hash = "OK"
    unverified |> verifyContactInfo hash

verifiedOk |> sendPasswordResetMessage


// errors
verifiedOk |> sendVerificationMessage
unverified |> sendPasswordResetMessage

