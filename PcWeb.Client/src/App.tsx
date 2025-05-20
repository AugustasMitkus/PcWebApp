import { HashRouter, Route, Routes} from "react-router-dom"
import 'bootstrap/dist/css/bootstrap.min.css'
import MemberGroups from "./components/MemberGroups"

function App() {
  return (
    <HashRouter>
      <Routes>
        <Route path="/" element={<MemberGroups />} />
      </Routes>
    </HashRouter>
  );
}

export default App;
