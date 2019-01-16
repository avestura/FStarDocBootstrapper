namespace OxFstar

module CoreTypes =
    open System
    open FSharp.Data

    let [<Literal>] Url = "https://www.fstar-lang.org/tutorial/tutorial.html"
    type FStarDocHtml = Url HtmlProvider

    type TocItem = {
        HeadingLabel : int []
        Text : string
    }

    type TocBlock = {
        Blocks : TocBlock []
        Items : TocItem []
    }

    type TocChild = 
        | TocBlock of TocBlock
        | TocItem of TocItem
