import { useState, useEffect } from 'react';
import { globalHistory } from '@reach/router';

export const useLocation = () => {
  const initialState = globalHistory.location;

  const [state, setState] = useState(initialState);
  useEffect(() => {
    const removeListener = globalHistory.listen(params => {
      const { location } = params;
      setState(location);
    });
    return () => {
      removeListener();
    };
  }, []);

  return state;
};

