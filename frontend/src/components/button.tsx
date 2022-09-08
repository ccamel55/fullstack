import "./button.css"

interface IButton {
    name: string
    callback: () => void
}

export default function Button(props:IButton) {
    return (
        <button className={"Button-" + props.name} onClick= {props.callback}>
            {props.name}
        </button>
    )
}