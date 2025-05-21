import { HashRouter, Route, Routes} from "react-router-dom"
import 'bootstrap/dist/css/bootstrap.min.css'
import GroupList from "./components/GroupList"
import MemberGroup from "./components/MemberGroup"
import Transaction from "./components/Transaction"

function App() {
  return (
    <HashRouter>
      <Routes>
        <Route path="/" element={<GroupList />} />
        <Route path="/group/:groupId" element={<MemberGroup />} />
        <Route path="/transaction/:groupId" element={<Transaction />} />
      </Routes>
    </HashRouter>
  );
}

export default App;
