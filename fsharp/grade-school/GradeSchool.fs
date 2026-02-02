module GradeSchool

type School = Map<int, string list>

let empty: School = Map.empty.Add(1, List.empty).Add(2, List.empty).Add(3, List.empty).Add(4, List.empty).Add(5, List.empty).Add(6, List.empty).Add(7, List.empty)

let add (student: string) (grade: int) (school: School): School =
    let sorted_grade_list = List.sort (student :: school.[grade])
    school.Add(grade, sorted_grade_list)
    

let roster (school: School): string list =  school.[1] @ school.[2] @ school.[3] @ school.[4] @ school.[5] @ school.[6] @ school.[7]

let grade (number: int) (school: School): string list = school.[number]
