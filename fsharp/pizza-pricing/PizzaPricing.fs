module PizzaPricing

type Pizza = 
    | Margherita
    | Caprese
    | Formaggio
    | ExtraSauce of Pizza
    | ExtraToppings of Pizza


let rec pizzaPrice (pizza: Pizza): int = 
    match pizza with 
    | Margherita -> 7
    | Caprese -> 9
    | Formaggio -> 10
    | ExtraSauce pizza -> 1 + pizzaPrice pizza
    | ExtraToppings pizza -> 2 + pizzaPrice pizza

let rec orderPrice(pizzas: Pizza list): int = 
    match pizzas with 
    | [] -> 0
    | [pizza] -> pizzaPrice pizza + 3
    | [pizza1; pizza2] -> pizzaPrice pizza1 + pizzaPrice pizza2 + 2
    | [pizza1; pizza2; pizza3] -> pizzaPrice pizza1 + pizzaPrice pizza2 + pizzaPrice pizza3
    | head :: rest -> pizzaPrice head + orderPrice rest


