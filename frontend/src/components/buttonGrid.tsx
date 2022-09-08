import "./buttonGrid.css"

interface IButtonGrid {
    children: React.ReactNode
}

export default function ButtonGrid(props:IButtonGrid) {
    return (
        <div className="ButtonGrid">
            {props.children}
        </div>
    );
}