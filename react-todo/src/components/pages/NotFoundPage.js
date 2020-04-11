import React from "react";
import { Link } from "react-router-dom";
// import PageNotFound from "../assets/images/PageNotFound";
class NotFoundPage extends React.Component {
  render() {
    return (
      <div>
        <h1>Sorry, there's now page at this address.</h1>
        <p style={{ textAlign: "center" }}>
          <Link to="/"> Home </Link>
        </p>
      </div>
    );
  }
}
export default NotFoundPage;
