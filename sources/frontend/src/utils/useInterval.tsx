import React, { useState, useEffect, useRef } from 'react';

export function useInterval(callback: () => void, delayMs: number) {
  const savedCallback = useRef<() => void | undefined>();

  // Remember the latest callback.
  useEffect(() => {
    savedCallback.current = callback;
  }, [callback]);

  // Set up the interval.
  useEffect(() => {
    function tick() {
      savedCallback.current?.();
    }
    if (delayMs !== null) {
      const id = setInterval(tick, delayMs);
      return () => clearInterval(id);
    }
  }, [delayMs]);
}
