import "./history.css"

interface IHistory {
    data: string
}

export default function History(props:IHistory) {
    return (
        <div className="History">
            <p>{props.data}</p>
            <p className="History-info">username time-stamp</p>
        </div>
    );
}