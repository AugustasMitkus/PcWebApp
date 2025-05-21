import { HashRouter, Route, Routes} from "react-router-dom"
import 'bootstrap/dist/css/bootstrap.min.css'
import GroupList from "./components/GroupList"
import MemberGroup from "./components/MemberGroup"

function App() {
  return (
    <HashRouter>
      <Routes>
        <Route path="/" element={<GroupList />} />
        <Route path="/group/:groupId" element={<MemberGroup />} />
      </Routes>
    </HashRouter>
  );
}

export default App;
