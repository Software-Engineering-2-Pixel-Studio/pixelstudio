// import logo from './logo.svg';
// import './App.css';

// function App() {
//   return (
//     <div className="App">
//       <header className="App-header">
//         <img src={logo} className="App-logo" alt="logo" />
//         <p>
//           Edit <code>src/App.js</code> and save to reload.
//         </p>
//         <a
//           className="App-link"
//           href="https://reactjs.org"
//           target="_blank"
//           rel="noopener noreferrer"
//         >
//           Learn React
//         </a>
//       </header>
//     </div>
//   );
// }

// export default App;


import React, { Component } from 'react';
import { BrowserRouter, Route, Switch } from 'react-router-dom';
import Navbar from './components/Navbar';
import About from './pages/About';
import Community from './pages/Community';
import Features from './pages/Features';
import Home from './pages/Home';
import Playroom from './pages/Playroom';
import Ranking from './pages/Ranking';
import Signin from './pages/Signin';
import Support from './pages/Support';


class App extends Component {
  render() {
    return (
      <BrowserRouter>
      <div className="App">
        <Navbar/>
        <Switch>
          <Route exact path="/" component={Home}/>
          <Route path="/signin" component={Signin}/>
          <Route path="/playroom" component={Playroom}/>
          <Route path="/ranking" component={Ranking}/>
          <Route path="/about" component={About}/>
          <Route path="/community" component={Community}/>
          <Route path="/Features" component={Features}/>
          <Route path="/support" component={Support}/>
        </Switch>
      </div>
      </BrowserRouter>
    );
  }
}
export default App;
