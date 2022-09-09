import "./history.css"

export interface IHistory {
    data: string
    userInfo: string
}

export default function History(props:IHistory) {
    return (
        <div className="History">
            <p>{props.data}</p>
            <p className="History-info">{props.userInfo}</p>
        </div>
    );
}