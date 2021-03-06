import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import React, { Component } from "react";
import Header from "./components/layout/Header";
import TodoList from "./components/pages/Todo/TodoList";
import AddTodo from "./components/pages/Todo/AddTodo";
import About from "./components/pages/About";
import ErrorBoundary from "./components/pages/ErrorBoundary";
// import ErrorBoundary from 'react-error-boundary';
import NotFoundPage from "./components/pages/NotFoundPage";
import "./App.css";
import axios from "axios";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

window.addEventListener("unhandledrejection", function (promiseRejectionEvent) {
  // handle error here, for example log
  //debugger;
  promiseRejectionEvent.preventDefault();
});

toast.configure({
  autoClose: 100,
  draggable: true,
  position: "bottom-left",
  //etc you get the idea
});

class App extends Component {
  state = {
    todos: [],
    url: "",
  };

  constructor() {
    super();
    this.url = process.env.REACT_APP_API_URL;
  }

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
    // Add a response interceptor
    axios.interceptors.response.use(
      function (response) {
        // debugger;
        return response;
      },
      function (error) {
        // debugger;
        if (error.response) {
          if (error.response.status >= 400) {
            toast.error(error.response.statusText);
          }
        } else {
          if (error.message === "Network Error") {
            toast.error("Network Error, please try again later.");
          } else {
            toast.error(error.message);
          }
        }

        return Promise.reject(error);
      }
    );

    axios.get(this.url).then((res) => this.setState({ todos: res.data }));
  }

  render() {
    return (
      <Router>
        <div className="App">
          <div className="container">
            <Header />
            <Switch>
              <Route
                path="/"
                exact
                render={(props) => (
                  <React.Fragment>
                    <ErrorBoundary>
                      <AddTodo addTodo={this.addTodo} />
                      <TodoList
                        todos={this.state.todos}
                        markComplete={this.markComplete}
                        delTodo={this.delTodo}
                      />
                    </ErrorBoundary>
                  </React.Fragment>
                )}
              />
              <Route path="/about" component={About} />
              <Route path="*" component={NotFoundPage} />
            </Switch>
            <ToastContainer />
          </div>
        </div>
      </Router>
    );
  }
}

export default App;
