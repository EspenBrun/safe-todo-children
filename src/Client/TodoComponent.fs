module Client.TodoComponent

open Elmish
open Fable.Remoting.Client
open Feliz
open Feliz.Bulma
open Shared

type Model = Todo

type Msg =
    | SetInput of string
    | TodoAdded of Todo

let todosApi =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.buildProxy<ITodosApi>

let init model =
    model, Cmd.none

let update msg model =
    match msg with
    | SetInput input ->
        let todo = { model with Description = input }
        todo, Cmd.OfAsync.perform todosApi.updateTodo todo TodoAdded
    | TodoAdded _ ->
        model, Cmd.none

let view model dispatch =
    Bulma.field.div [
        field.isGrouped
        prop.children [
            Bulma.control.p [
                control.isExpanded
                prop.children [
                    Bulma.input.text [
                        prop.value model.Description
                        prop.placeholder "What needs to be done?"
                        prop.onChange (fun x -> SetInput x |> dispatch)
                    ]
                ]
            ]
        ]
    ]
