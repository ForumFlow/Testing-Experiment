import React from 'react';

function Comment({ text, upvotes, downvotes, id, parentId }) {
  if (!text) {
    return null; // or handle the case where text is undefined
  }

  return (
    <div className="comment">
      <p>{text}</p>
      <div>
        <span>Upvotes: {upvotes}</span>
        <span>Downvotes: {downvotes}</span>
        <span>ID: {id}</span>
        <span>Parent ID: {parentId}</span>
      </div>
    </div>
  );
}

export default Comment;