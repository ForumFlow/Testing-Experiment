import './App.css';
import React, { useState } from 'react';
import NewPresentation from './components/NewPresentation';
import Presentation from './components/Presentation';
import Comment from './components/comment';
import AddComment from './components/AddComment';

function App() {
  const [comments, setComments] = useState([]);
  const [buttonClicked, setButtonClicked] = useState(false);
  const [presentation, setPresentation] = useState(null);

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

  const handleAddPresentation = () => {
    if (!buttonClicked){
    setButtonClicked(true);
    }else{
      setButtonClicked(false);
    }
  };
  const handleNewPresentation = (newPresentation) => {
    setPresentation(newPresentation);
    setButtonClicked(false);
  };
  return (
    <div className="App">
      {!buttonClicked && (
        <button onClick={handleAddPresentation}>Add Presentation</button>
      )}
      {buttonClicked ? (
        <NewPresentation onSubmit={handleNewPresentation} />
      ) : (
        <>
          {presentation ? (
            <Presentation 
              author={presentation.author} 
              text={presentation.text} 
              title={presentation.title} 
            />
          ) : (
        <Presentation author="Presenter's name" text="An faq/forum web application where people can create a forum post, share a 
        link via a qr code, and people can comment and upvote on the forum post. 
        Our main application of this would allow people/ presenters to provide information to users/ helping informative." title="sample title"/>
          )}
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
        </>
      )}
    </div>
  );
}

export default App;