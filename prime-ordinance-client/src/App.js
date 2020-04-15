import React, { useEffect, useRef } from 'react';
import logo from './logo.svg';
import './App.css';

function App() {
  const canvas = useRef(null);
  useEffect(() => {
    canvas.current.requestPointerLock();
  });

  return (
    <svg ref={canvas} className="canvas">
        <g>
          <circle className="zone" cx="200" cy="200" r="100"/>
          <circle className="originator" cx="200" cy="200" r="4">
                <animate attributeName="r" values="4;6;4" dur="1s" repeatCount="indefinite" />
                <animate attributeName="stroke-width" values="2;6;2" dur="1s" repeatCount="indefinite" />
          </circle>
        </g>

        <g>
          <circle className="path-point" cx="300" cy="300" r="5" />
          <circle className="path-point" cx="400" cy="300" r="5" />
          <path className="path" d="M400 200 L300 300 L400 300" />
        </g>

        
    </svg>
  );
}

export default App;
