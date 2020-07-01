import React, {Component} from 'react';
import './App.css';
import axios from 'axios';
import ResultComponent from './Components/ResultComponent';
import KeyPadComponent from "./Components/KeyPadComponent";

export default class App extends Component {
    constructor(){
        super();
        this.state = {
            result: ""
        }
    }
    
    onClick = button => {
      if(button === "="){
          // call to server
          var api_gateway_url = "http://webapiforreactcalculator-dev.us-east-1.elasticbeanstalk.com/getResult";
          var payload = {
            "text" : this.state.result
          }
          axios.post(api_gateway_url, payload).
          then(res => {
            if(res.statusText === "OK")
            {
              this.setState({
                result : res.data
              });
            }
            else{
              this.setState({
                result : "server error"
              });
            }
          })
      }
      else if(button === "C"){
          this.reset()
      }
      else if(button === "CE"){
          this.backspace()
      }
      else {
          this.setState({
              result: this.state.result + button
          })
      }
    };
    backspace = () => {
        this.setState({
            result: this.state.result.slice(0, -1)
        });
    };
    reset = () => {
        this.setState({
            result: ""
        });
    };

    render() {
        return (
            <div>
                <div className="calculator-body">
                    <ResultComponent result={this.state.result}/>
                    <KeyPadComponent onClick={this.onClick}/>
                </div>
            </div>
        );
    }
}
