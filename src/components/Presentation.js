import React from 'react';

export default function Presentation(props) {
    return (
        <div>
            <h1>{props.author}</h1>
            <h1>{props.title}</h1>
            <p>{props.text}</p>
        </div>
    );
}