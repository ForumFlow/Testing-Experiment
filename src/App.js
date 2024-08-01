import './App.css';
import React, { useState } from 'react';
import Presentation from './components/Presentation';
import Comment from './components/comment';

function AddComment({ onSubmit, parentId }) {
  const [newCommentText, setNewCommentText] = useState("");
  const [isTextFieldVisible, setIsTextFieldVisible] = useState(false);

  const handleAddCommentClick = () => {
    setIsTextFieldVisible(true);
  };

  const handleSubmitComment = () => {
    if (newCommentText.trim() !== "") {
      onSubmit(newCommentText, parentId);
      setNewCommentText(""); // Clear the input field after adding the comment
      setIsTextFieldVisible(false); // Hide the text field after submitting the comment
    }
  };

  return (
    <div>
      {isTextFieldVisible ? (
        <div>
          <input 
            type="text" 
            value={newCommentText} 
            onChange={(e) => setNewCommentText(e.target.value)} 
            placeholder="Enter your comment"
          />
          <button onClick={handleSubmitComment}>Submit Comment</button>
        </div>
      ) : (
        <button onClick={handleAddCommentClick}>Add Comment</button>
      )}
    </div>
  );
}

function App() {
  const [comments, setComments] = useState([]);

  const handleAddComment = (text, parentId = null) => {
    const newComment = {
      id: comments.length + 1, // Simple unique ID generation
      text,
      parentId,
      upvotes: 0,
      downvotes: 0
    };
    setComments([...comments, newComment]);
  };

  return (
    <div className="App">
      <Presentation author="Presenter's name" text="An faq/forum web application where people can create a forum post, share a 
      link via a qr code, and people can comment and upvote on the forum post. 
      Our main application of this would allow people/ presenters to provide information to users/ helping informative." title="sample title"/>
      <AddComment onSubmit={handleAddComment} parentId={null}/>
      {comments.map((comment) => (
        <div key={comment.id}>
          <Comment 
            text={comment.text} 
            upvotes={comment.upvotes} 
            downvotes={comment.downvotes} 
            id={comment.id}
            parentId={comment.parentId}
          />
          <AddComment onSubmit={handleAddComment} parentId={comment.id}/>
        </div>
      ))}
    </div>
  );
}

export default App;