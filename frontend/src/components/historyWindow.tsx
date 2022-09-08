import "./historyWindow.css"

interface IHistoryWindow {
    children: React.ReactNode
}

export default function Window(props:IHistoryWindow) {
    return (
        <div className={"HistoryWindow"}>
            {props.children}
        </div>
    );
}