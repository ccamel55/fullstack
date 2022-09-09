import React, { useState } from 'react';
import ReactDOM from 'react-dom/client';

import "./index.css"

import Window from './components/window';
import Text from './components/text';
import Button from './components/button';
import ButtonGrid from './components/buttonGrid';
import HistoryWindow from './components/historyWindow';
import History from './components/history';
import Popup from './components/popup';
import TextField from './components/textField';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

function App() {

  const [usePopup, setUsePopup] = useState(true);
  const [userName, setUserName] = useState(null);

  const [calc, setCalc] = useState("");
  const [wasCalc, setWasCalc] = useState(false);

  const USERNAME_TEXT_ID = "username_text_id";

  const updateTextData = (event:any) => {
    setUserName(event.target.value);
  }

  const processUsername = () => {

    if (userName == null)
      return;
    
      setUsePopup(false);
  }

  const updateCalc = (value:any) => {

    const operators = ["+", "-", "*", "/"]

    // only allow one operator
    if ((operators.includes(value) && calc === "") || 
        (operators.includes(value) && operators.includes(calc.slice(-1)))) {

          // update the operator
          setCalc(calc.substring(0, calc.length -1) + value);
          return;
        }
    
    // if we are working on a result only allow operators
    if (wasCalc && !operators.includes(value))
      return;

    // work on result
    setCalc(calc + value);
    setWasCalc(false);
  }

  const updateResult = () => {
    setWasCalc(true);
    setCalc(eval(calc).toString());
  }

  return (
    <div className="App">
      <Popup open={usePopup}>
        <h2>Enter a username</h2>
        <TextField id={USERNAME_TEXT_ID} callback={updateTextData}/>
        <Button name='Continue' callback={processUsername}/>
      </Popup>

      <HistoryWindow>
        <History data={"some calc"}/>
      </HistoryWindow>

      <Window>
        <Text str= { calc === "" ? "0" : calc }/>
        <ButtonGrid>
          <Button name='+' callback={() => { updateCalc("+"); }}/>
          <Button name='-' callback={() => { updateCalc("-"); }}/>
          <Button name='x' callback={() => { updateCalc("*"); }}/>
          <Button name='/' callback={() => { updateCalc("/"); }}/>

          <Button name='1' callback={() => { updateCalc("1"); }}/>
          <Button name='2' callback={() => { updateCalc("2"); }}/>
          <Button name='3' callback={() => { updateCalc("3"); }}/>
          <Button name='4' callback={() => { updateCalc("4"); }}/>

          <Button name='5' callback={() => { updateCalc("5"); }}/>
          <Button name='6' callback={() => { updateCalc("6"); }}/>     
          <Button name='7' callback={() => { updateCalc("7"); }}/>
          <Button name='8' callback={() => { updateCalc("8"); }}/>

          <Button name='9' callback={() => { updateCalc("9") }}/>
          <Button name='0' callback={() => { updateCalc("0") }}/>
          <Button name='C' callback={() => { setCalc(""); setWasCalc(false); }}/>
          <Button name='=' callback={() => { updateResult(); }}/>
        </ButtonGrid>
      </Window>
    </div>
  );
}

root.render(
  <React.StrictMode>
    <App/>
  </React.StrictMode>
);

