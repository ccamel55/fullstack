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

  const USERNAME_TEXT_ID = "username_text_id";

  const updateTextData = (event:any) => {
    setUserName(event.target.value);
  }

  const processUsername = () => {

    if (userName == null)
      return;
    
      setUsePopup(false);
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
        <History data={"some calc"}/>
        <History data={"some calc"}/>
        <History data={"some calc"}/>
        <History data={"some calc"}/>
        <History data={"some calc"}/>
        <History data={"some calc"}/>
        <History data={"some calc"}/>
        <History data={"some calc"}/>
        <History data={"some calc"}/>
        <History data={"some calc"}/>
        <History data={"some calc"}/>
      </HistoryWindow>

      <Window>
        <Text str="Output"/>
        <ButtonGrid>
          <Button name='+' callback={() => {}}/>
          <Button name='-' callback={() => {}}/>
          <Button name='x' callback={() => {}}/>
          <Button name='/' callback={() => {}}/>

          <Button name='1' callback={() => {}}/>
          <Button name='2' callback={() => {}}/>
          <Button name='3' callback={() => {}}/>
          <Button name='4' callback={() => {}}/>

          <Button name='5' callback={() => {}}/>
          <Button name='6' callback={() => {}}/>     
          <Button name='7' callback={() => {}}/>
          <Button name='8' callback={() => {}}/>

          <Button name='9' callback={() => {}}/>
          <Button name='0' callback={() => {}}/>
          <Button name='C' callback={() => {}}/>
          <Button name='=' callback={() => {}}/>
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

