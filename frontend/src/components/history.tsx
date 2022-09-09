import "./history.css"

export interface IHistory {
    calculation: number;
    user: string;
    timeStamp: string;
}

export default function History(props:IHistory) {
    return (
        <div className="History">
            <p>{props.calculation}</p>
            <p className="History-info">{props.user}</p>
        </div>
    );
}