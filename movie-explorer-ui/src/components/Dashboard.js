import React from "react";

function Dashboard({ likedCount, searchCount, pageNumber }) {
  return (
    <div className="row mb-4">

      <div className="col-md-4">
        <div className="card text-center shadow-sm border-0">
          <div className="card-body">
            <h5>❤️ Liked Movies</h5>
            <h2>{likedCount}</h2>
          </div>
        </div>
      </div>

      <div className="col-md-4">
        <div className="card text-center shadow-sm border-0">
          <div className="card-body">
            <h5>🎬 Movies Found</h5>
            <h2>{searchCount}</h2>
          </div>
        </div>
      </div>

      <div className="col-md-4">
        <div className="card text-center shadow-sm border-0">
          <div className="card-body">
            <h5>📄 Current Page</h5>
            <h2>{pageNumber}</h2>
          </div>
        </div>
      </div>

    </div>
  );
}

export default Dashboard;