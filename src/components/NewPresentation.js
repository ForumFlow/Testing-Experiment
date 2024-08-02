import React, { useState } from 'react';
import Presentation from './Presentation';

function NewPresentation({ onSubmit }) {
  const [author, setAuthor] = useState('');
  const [text, setText] = useState('');
  const [title, setTitle] = useState('');
  const [submitted, setSubmitted] = useState(false);

  const handleSubmit = (e) => {
    e.preventDefault();
    setSubmitted(true);
    onSubmit({ author, text, title });
  };

  return (
    <div>
      <h2>Create a New Presentation</h2>
      {submitted ? (
        <Presentation author={author} text={text} title={title} />
      ) : (
        <form onSubmit={handleSubmit}>
          <div>
            <label>Author:</label>
            <input 
              type="text" 
              value={author} 
              onChange={(e) => setAuthor(e.target.value)} 
            />
          </div>
          <div>
            <label>Text:</label>
            <textarea 
              value={text} 
              onChange={(e) => setText(e.target.value)} 
            />
          </div>
          <div>
            <label>Title:</label>
            <input 
              type="text" 
              value={title} 
              onChange={(e) => setTitle(e.target.value)} 
            />
          </div>
          <button type="submit">Submit</button>
        </form>
      )}
    </div>
  );
}

export default NewPresentation;