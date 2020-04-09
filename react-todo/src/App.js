import { BrowserRouter as Router, Route } from "react-router-dom";
import React, { Component } from "react";
import Header from "./components/layout/Header";
import Todos from "./components/Todos";
import AddTodo from "./components/AddTodo";
import About from "./components/pages/About";
// import { v4 as uuid } from "uuid";
import "./App.css";
import axios from "axios";

class App extends Component {
  state = {
    todos: [],
  };

  url = "http://localhost:5000/api/todo";

  // add new task
  addTodo = (description) => {
    let data = {
      description,
      isCompleted: false,
    };
    axios
      .post(this.url, data)
      .then((res) =>
        this.setState(this.setState({ todos: [...this.state.todos, res.data] }))
      );
  };

  // Toggle Complete
  markComplete = (id) => {
    axios.put(`${this.url}/${id}`).then((res) =>
      this.setState({
        todos: this.state.todos.map((todo) => {
          if (todo.id === id) {
            todo.isCompleted = !todo.isCompleted;
          }
          return todo;
        }),
      })
    );

    // this.setState({
    //  todos: this.state.todos.map((todo) => {
    //    if (todo.id === id) {
    //      todo.isCompleted = !todo.isCompleted;
    //    }
    //    return todo;
    //  }),
    // });
  };

  // delete task
  delTodo = (id) => {
    axios.delete(`${this.url}/${id}`).then((res) =>
      this.setState({
        todos: [...this.state.todos.filter((todo) => todo.id !== id)],
      })
    );
  };

  componentDidMount() {
    axios.get(this.url).then((res) => this.setState({ todos: res.data }));
    // .catch(() => console.log("Can't access url"));
  }

  render() {
    return (
      <Router>
        <div className="App">
          <div className="container">
            <Header />
            <Route
              exact
              path="/"
              render={(props) => (
                <React.Fragment>
                  <AddTodo addTodo={this.addTodo} />
                  <Todos
                    todos={this.state.todos}
                    markComplete={this.markComplete}
                    delTodo={this.delTodo}
                  />
                </React.Fragment>
              )}
            />
            <Route path="/about" component={About} />
          </div>
        </div>
      </Router>
    );
  }
}

export default App;
