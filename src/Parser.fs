namespace OxFStar
open FSharp.Data
open System.Text.RegularExpressions

module Parser =
    open System
    open OxFStar.CoreTypes

    let ItemNodesOf (n:HtmlNode) = n.CssSelect "div.tocitem"
    let BlockNodesOf (n:HtmlNode) = n.CssSelect "div.tocblock"

    let ParseDoc (doc:HtmlDocument) =
        let tocRoot = (doc.CssSelect "nav.toc.toc-contents > div.tocblock").Head

        let convertToItem (n : HtmlNode) =
            let tocLine = (n.Attribute "data-toc-line").Value ()
            let lineParseRegex = Regex @"\[((\d\.)*\d)\]\{\.[a-zA-Z-]+\}\.[ ](.*)?"
            let matched = lineParseRegex.Match tocLine
            let toArrayForamt (x:string) = (x.Split [|'.'|]) |> Seq.map Int32.Parse
            {
                HeadingLabel = toArrayForamt matched.Groups.[0].Value :?> int []
                Text = matched.Groups.[2].Value
            }

        let rec convertToBlock (n : HtmlNode) =
            {
                Blocks = (BlockNodesOf n) |> List.map convertToBlock |> List.toArray
                Items  = (ItemNodesOf n)  |> List.map convertToItem |> List.toArray
            }

        convertToBlock tocRoot