import "./textField.css"

interface ITextField {
    id: string
    callback: (event:any) => void
}

export default function TextField(props:ITextField) {
    return (
        <input id={props.id} type="text" maxLength={20} onChange={props.callback}/>
    );  
}