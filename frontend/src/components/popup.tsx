import "./popup.css" 

interface IPopup {
    open: boolean
    children: React.ReactNode
}

export default function Popup(props:IPopup) {
    return props.open ? (
        <div className="Popup">
            <div className="Popup-inner">
                {props.children}
            </div>
        </div>
    ) : (<></>);
}