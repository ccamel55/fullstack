import "./text.css"

interface IText {
    str: string
}

export default function Text(props: IText) {
    return (
        <h2 className={"Text"}>
            {props.str}
        </h2>
    )
}