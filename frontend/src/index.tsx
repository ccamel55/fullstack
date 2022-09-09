import React, { useEffect, useState } from 'react';
import ReactDOM from 'react-dom/client';
import axios from "axios";

import "./index.css"

import Window from './components/window';
import Text from './components/text';
import Button from './components/button';
import ButtonGrid from './components/buttonGrid';
import HistoryWindow from './components/historyWindow';
import Popup from './components/popup';
import TextField from './components/textField';
import History, {IHistory} from './components/history';

const REFRESH_MS = 30000;
const API_URL = "http://allanterrabackend.azurewebsites.net/calc";

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

function App() {

  const [usePopup, setUsePopup] = useState(true);
  const [userName, setUserName] = useState("");
  const [apiError, setApiError] = useState(false);
  const [history, setHistory] = useState<IHistory[]>([]);

  const [calc, setCalc] = useState("");
  const [result, setResult] = useState("");

  const USERNAME_TEXT_ID = "username_text_id";

  const updateTextData = (event:any) => {
    setUserName(event.target.value);
  }

  const processUsername = () => {

    if (userName === "")
      return;
    
      setUsePopup(false);
      console.log(userName);
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
    if (result !== "") {
      if (!operators.includes(value))
        return
      
      setCalc(result + value);
      setResult("");
    
      return;
    }

    // work on result
    setCalc(calc + value);
  }

  const updateResult = () => {
    if (calc === "")
      return;

    var resultStr = (Math.round(eval(calc) * 100) / 100).toString();
    setResult(resultStr);

    axios.post(API_URL + "?" + new URLSearchParams({ username: userName,  calculation: calc + "=" + resultStr}).toString())
    .catch((error) => {
      console.log(error);
    });
  }

  // fetch history from server
  const updateHistoryBox = () => {
    
    axios.get<IHistory[]>(API_URL).then((response) => {
      
      setApiError(false);
      setHistory(response.data.reverse());
    })
    .catch((error) => {
      console.log(error);
      setApiError(true);
    })
  }

  useEffect(() => {

    // on init refresh once
    updateHistoryBox();

    // auto refresh ever 60 seconds
    const interval = setInterval(() => {
      updateHistoryBox();
    }, REFRESH_MS);
  
    return () => clearInterval(interval);
  }, [])

  return (
    <div className="App">
      <Popup open={usePopup}>
        <h2>Enter a username</h2>
        <TextField id={USERNAME_TEXT_ID} callback={updateTextData}/>
        <Button name='Continue' callback={processUsername}/>
      </Popup>

      <HistoryWindow>
        { apiError ? <p>API ERROR</p> :  
        history.map(({user, calculation, timeStamp}:IHistory) => {
          return <History calculation={calculation} user={user} timeStamp={timeStamp}/>
        })}
      </HistoryWindow>

      <Window>
        <Text str= { result === "" ? calc === "" ? "0" : calc : result }/>
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
          <Button name='C' callback={() => { setCalc(""); setResult(""); }}/>
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

