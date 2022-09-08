import "./window.css"

interface IWindow {
    children: React.ReactNode
}

export default function Window(props:IWindow) {
    return (
        <div className={"Window"}>
            {props.children}
        </div>
    );
}