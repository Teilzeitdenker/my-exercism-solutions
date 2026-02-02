module LensPerson

open System
open Aether.Operators 

type Name = { name: string; surName: string }
module Name = 
    let name_ = (fun n -> n.name), (fun m n -> { n with name = m })
    let surName_ = (fun n -> n.surName), (fun m n -> { n with surName = m })
type Address = { street: string; houseNumber: int; place: string; country: string }
module Address = 
    let street_ = (fun a -> a.street), (fun s a -> { a with street = s })
    let houseNumber_ = (fun a -> a.houseNumber), (fun h a -> { a with houseNumber = h })
    let place_ = (fun a -> a.place), (fun p a -> { a with place = p })
    let country_ = (fun a -> a.country), (fun c a -> { a with country = c })
type BirthPlaceAndDate = { at: Address; on: DateTime }
module BirthPlaceAndDate = 
    let at_ = (fun b -> b.at), (fun a b -> { b with at = a })
    let on_ = (fun b -> b.on), (fun o b -> { b with on = o })
    let onMonth_ = (fun b -> b.on.Month), (fun m b -> { b with on = new DateTime(b.on.Year, m, b.on.Day) })
type Person = { name: Name; born: BirthPlaceAndDate; address: Address }
module Person = 
    let name_ = (fun p -> p.name), (fun n p -> { p with name = n })
    let born_ = (fun p -> p.born), (fun b p -> { p with born = b })
    let address_ = (fun p -> p.address), (fun a p -> { p with address = a })

let bornAtStreet = Person.born_ >-> BirthPlaceAndDate.at_ >-> Address.street_
let currentStreet = Person.address_ >-> Address.street_ 
let bornOn = Person.born_ >-> BirthPlaceAndDate.on_ 
let birthMonth = Person.born_ >-> BirthPlaceAndDate.onMonth_